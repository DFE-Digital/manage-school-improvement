// src/Dfe.ManageSchoolImprovement/wwwroot/src/scrollPosition.js

(function () {
    const HISTORY_KEY = 'navigation_history';
    const MAX_HISTORY = 10;
    const SCHOOLS_LIST_PATH = '/schools-identified-for-targeted-intervention';

    // Get history from session storage
    function getNavigationHistory() {
        const history = sessionStorage.getItem(HISTORY_KEY);
        return history ? JSON.parse(history) : [];
    }

    // Save history to session storage
    function saveNavigationHistory(history) {
        sessionStorage.setItem(HISTORY_KEY, JSON.stringify(history));
    }

    function saveScrollPosition() {
        const key = `scroll_${window.location.pathname}`;
        sessionStorage.setItem(key, window.scrollY);

        // Always save schools list scroll position with a special key
        if (window.location.pathname === SCHOOLS_LIST_PATH) {
            sessionStorage.setItem('schools_list_scroll', window.scrollY);
        }
    }

    function restoreScrollPosition() {
        const currentPath = window.location.pathname;

        // Special handling for schools list page
        if (currentPath === SCHOOLS_LIST_PATH) {
            const scrollY = sessionStorage.getItem('schools_list_scroll');
            if (scrollY !== null) {
                setTimeout(() => {
                    window.scrollTo(0, parseInt(scrollY));
                    // Don't remove the schools list scroll position
                }, 100);
                return;
            }
        }

        // Normal handling for other pages
        const key = `scroll_${currentPath}`;
        const scrollY = sessionStorage.getItem(key);

        if (scrollY !== null) {
            setTimeout(() => {
                window.scrollTo(0, parseInt(scrollY));
                sessionStorage.removeItem(key);
            }, 100);
        }
    }

    // Track page visits
    function addToHistory(path) {
        const history = getNavigationHistory();

        // Don't add if it's the same as the last entry
        if (history.length > 0 && history[history.length - 1] === path) {
            return;
        }

        history.push(path);
        if (history.length > MAX_HISTORY) {
            history.shift();
        }
        saveNavigationHistory(history);
    }

    // Check if this is a back navigation
    function isBackNavigation(currentPath) {
        // Always restore scroll for schools list page
        if (currentPath === SCHOOLS_LIST_PATH) {
            return true;
        }

        const history = getNavigationHistory();
        if (history.length < 3) return false;

        const previousPath = history[history.length - 3];
        return previousPath === currentPath;
    }

    // Handle all link clicks within the site
    function handleLinkClick(event) {
        const link = event.target.closest('a');
        if (!link) return;

        // Only handle internal links
        if (link.host === window.location.host) {
            saveScrollPosition();
        }
    }

    // Initialize
    function init() {
        // Track current page
        addToHistory(window.location.pathname);

        // Save position before navigating away
        window.addEventListener('beforeunload', saveScrollPosition);

        // Handle link clicks
        document.addEventListener('click', handleLinkClick);

        // Check for back navigation on page load
        window.addEventListener('DOMContentLoaded', () => {
            const isBackFromBrowser =
                window.performance?.navigation?.type === 2 ||
                window.performance?.getEntriesByType("navigation")[0]?.type === 'back_forward';

            const isBackFromLink = isBackNavigation(window.location.pathname);

            if (isBackFromBrowser || isBackFromLink) {
                restoreScrollPosition();
            }
        });

        // Update history after navigation
        window.addEventListener('popstate', () => {
            addToHistory(window.location.pathname);
        });
    }

    // Start tracking
    init();
})();
