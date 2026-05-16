window.DashboardNavigation = (function () {
    var button = null;

    function getParentPath() {
        var path = window.location.pathname.replace(/\/+$/, '');
        if (path === '' || path === '/') return null;
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