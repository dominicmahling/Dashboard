window.DashboardNavigation = (function () {
    var button = null;

    function getParentPath() {
        var path = window.location.pathname.replace(/\/+$/, '') || '/';

        if (path === '/') return null;

        if (path.match(/^\/articles\/\d+\/edit$/)) return '/articles/' + path.split('/')[2];
        if (path.match(/^\/articles\/\d+$/)) {
            return null;
        }
        if (path.match(/^\/topics\/\d+\/edit$/)) return '/topics/' + path.split('/')[2];
        if (path.match(/^\/topics\/\d+\/articles\/new$/)) return '/topics/' + path.split('/')[2];
        if (path.match(/^\/topics\/\d+$/)) return '/topics';
        if (path === '/topics/new') return '/topics';
        if (path === '/topics') return '/';
        if (path === '/search') return '/';

        var lastSlash = path.lastIndexOf('/');
        if (lastSlash <= 0) return '/';
        return path.substring(0, lastSlash) || '/';
    }

    function updateButton() {
        if (!button) return;
        var parent = getParentPath();
        button.style.display = parent ? '' : 'none';
        if (parent) {
            button.href = parent;
        }
    }

    function init() {
        button = document.querySelector('.back-button');

        var originalPushState = history.pushState.bind(history);
        history.pushState = function () {
            var result = originalPushState.apply(this, arguments);
            updateButton();
            return result;
        };

        window.addEventListener('popstate', function () {
            updateButton();
        });

        updateButton();
    }

    init();

    return { updateButton: updateButton };
})();