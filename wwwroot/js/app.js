window.DashboardNavigation = (function () {
    var button = null;
    var depth = 1;
    var suppressPopstate = false;
    var STORAGE_KEY = '_navDepth';

    function init() {
        button = document.querySelector('.back-button');
        depth = parseInt(sessionStorage.getItem(STORAGE_KEY) || '1', 10);

        var originalPushState = history.pushState.bind(history);
        history.pushState = function () {
            var result = originalPushState.apply(this, arguments);
            depth++;
            sessionStorage.setItem(STORAGE_KEY, depth.toString());
            updateButton();
            return result;
        };

        window.addEventListener('popstate', function () {
            if (suppressPopstate) {
                suppressPopstate = false;
                updateButton();
                return;
            }
            if (depth > 1) depth--;
            sessionStorage.setItem(STORAGE_KEY, depth.toString());
            updateButton();
        });

        updateButton();
    }

    function goBack() {
        if (depth <= 1) return;
        depth--;
        sessionStorage.setItem(STORAGE_KEY, depth.toString());
        suppressPopstate = true;
        history.back();
        updateButton();
    }

    function updateButton() {
        if (!button) return;
        button.style.display = depth > 1 ? '' : 'none';
    }

    init();

    return { goBack: goBack };
})();