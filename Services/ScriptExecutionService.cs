using System.Diagnostics;

namespace Dashboard.Services;

public class ScriptExecutionService
{
    public async Task<ScriptResult> ExecuteAsync(string script, int timeoutSeconds = 60)
    {
        var output = new List<string>();
        var errors = new List<string>();
        int exitCode;

        var psi = new ProcessStartInfo
        {
            FileName = "powershell.exe",
            Arguments = "-NoProfile -ExecutionPolicy Bypass -",
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            StandardOutputEncoding = System.Text.Encoding.UTF8,
            StandardErrorEncoding = System.Text.Encoding.UTF8
        };

        using var process = new Process { StartInfo = psi };
        process.Start();

        using (var sw = process.StandardInput)
        {
            if (sw.BaseStream.CanWrite)
            {
                await sw.WriteLineAsync(script);
                sw.WriteLineAsync("$LASTEXITCODE").GetAwaiter().GetResult();
            }
        }

        var outputTask = Task.Run(async () =>
        {
            string? line;
            while ((line = await process.StandardOutput.ReadLineAsync()) != null)
            {
                lock (output)
                {
                    output.Add(line);
                }
            }
        });

        var errorTask = Task.Run(async () =>
        {
            string? line;
            while ((line = await process.StandardError.ReadLineAsync()) != null)
            {
                lock (errors)
                {
                    errors.Add(line);
                }
            }
        });

        if (!process.WaitForExit(timeoutSeconds * 1000))
        {
            try { process.Kill(); } catch { }
            return new ScriptResult
            {
                Success = false,
                Output = string.Join(Environment.NewLine, output),
                Error = "Script timed out after " + timeoutSeconds + " seconds."
            };
        }

        await Task.WhenAll(outputTask, errorTask);

        exitCode = process.ExitCode;

        return new ScriptResult
        {
            Success = exitCode == 0,
            ExitCode = exitCode,
            Output = string.Join(Environment.NewLine, output),
            Error = string.Join(Environment.NewLine, errors)
        };
    }
}

public class ScriptResult
{
    public bool Success { get; set; }
    public int ExitCode { get; set; }
    public string Output { get; set; } = "";
    public string Error { get; set; } = "";
}
