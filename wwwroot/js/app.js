window.DashboardNavigation = (function () {
    let depth = 1;
    let button = null;
    let suppressPopstate = false;
    const STORAGE_KEY = '_navDepth';

    function init() {
        button = document.querySelector('.back-button');
        depth = parseInt(sessionStorage.getItem(STORAGE_KEY) || '1', 10);

        const originalPushState = history.pushState.bind(history);
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
            if (depth > 1) {
                depth--;
                sessionStorage.setItem(STORAGE_KEY, depth.toString());
            }
            updateButton();
        });

        updateButton();
    }

    function goBack() {
        if (depth <= 1) return;
        depth--;
        sessionStorage.setItem(STORAGE_KEY, depth.toString());
        suppressPopstate = true;
        window.history.back();
        updateButton();
    }

    function updateButton() {
        if (button) {
            button.style.display = depth > 1 ? '' : 'none';
        }
    }

    init();

    return { goBack: goBack };
})();