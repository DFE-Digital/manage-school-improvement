document.addEventListener("DOMContentLoaded", function () {
    document.querySelectorAll(".govuk-task-list__item--with-link").forEach(function (item) {
        const link = item.querySelector("a.govuk-link");
        if (link) {
            item.style.cursor = "pointer";
            item.addEventListener("click", function (event) {
                if (!event.target.closest("a")) {
                    window.location.href = link.href;
                }
            });
        }
    });
});
