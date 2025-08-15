// src/Dfe.ManageSchoolImprovement/wwwroot/src/scrollPosition.js

(function () {
    const HISTORY_KEY = 'navigation_history';
    const MAX_HISTORY = 10;
    const SCHOOLS_LIST_PATH = '/schools-identified-for-targeted-intervention';
    const SUPPRESS_SAVE_KEY = 'suppress_next_schools_scroll_save';

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
        // If we've explicitly suppressed the next save (e.g. after applying filters), skip saving
        const suppressSave = sessionStorage.getItem(SUPPRESS_SAVE_KEY) === 'true';
        if (suppressSave && window.location.pathname === SCHOOLS_LIST_PATH) {
            sessionStorage.removeItem(SUPPRESS_SAVE_KEY);
            return;
        }

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

    function handleFilterSubmit(event) {
        // Clear the stored scroll position for the schools list
        sessionStorage.removeItem('schools_list_scroll');
        // Set it to 0 to ensure top position
        sessionStorage.setItem('schools_list_scroll', '0');

        // Prevent the upcoming navigation from re-saving the current scroll position
        sessionStorage.setItem(SUPPRESS_SAVE_KEY, 'true');
        window.removeEventListener('beforeunload', saveScrollPosition);
    }

    // Initialize
    function init() {
        function bindFilterButton() {
            const filterForm = document.querySelector('#filter-submit');
            if (filterForm) {
                filterForm.addEventListener('click', handleFilterSubmit);
            }
        }

        if (document.readyState === 'loading') {
            document.addEventListener('DOMContentLoaded', bindFilterButton);
        } else {
            bindFilterButton();
        }

        // Track current page
        addToHistory(window.location.pathname);

        // Save position before navigating away
        window.addEventListener('beforeunload', saveScrollPosition);

        // Handle link clicks
        document.addEventListener('click', handleLinkClick);

        // Check for back navigation on page load
        function restoreScroll() {
            const isBackFromBrowser =
                window.performance?.navigation?.type === 2 ||
                window.performance?.getEntriesByType("navigation")[0]?.type === 'back_forward';

            const isBackFromLink = isBackNavigation(window.location.pathname);

            if (isBackFromBrowser || isBackFromLink) {
                restoreScrollPosition();
            }
        }
        
        if (document.readyState === 'loading') {
            window.addEventListener('DOMContentLoaded', restoreScroll);
        } else {
            restoreScroll();
        }

        // Update history after navigation
        window.addEventListener('popstate', () => {
            addToHistory(window.location.pathname);
        });
    }

    // Start tracking
    init();
})();
