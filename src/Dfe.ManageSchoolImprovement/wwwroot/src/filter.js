document.addEventListener('DOMContentLoaded', function () {
   // If this method is called JS is enabled, thus boxes should be shown
   showFilters();
   setupFilters();

   function setupFilters() {
      const filterSections = document.querySelectorAll('.govuk-accordion__section');
      filterSections.forEach(section => {
         const searchInput = section.querySelector('.govuk-input');
         const items = section.querySelectorAll('.govuk-checkboxes__item');

         if (searchInput) {
            searchInput.addEventListener('keyup', function (e) {
               const searchValue = e.target.value.toLowerCase();
               filterItems(items, searchValue);
            });
         }
      });
   }
   function showFilters() {
      // Select all input elements with the 'govuk-!-display-none' class
      const inputs = document.querySelectorAll('input.govuk-\\!-display-none');

      // Iterate over each input and remove the 'govuk-!-display-none' class to display them
      inputs.forEach(function (input) {
         input.classList.remove('govuk-!-display-none');
      });
   }

   function filterItems(items, searchValue) {
      items.forEach(function (item) {
         const itemName = item.textContent.toLowerCase();
         if (itemName.includes(searchValue)) {
            item.style.display = '';
         } else {
            item.style.display = 'none';
         }
      });
   }
   
   const yearCheckboxes = document.querySelectorAll('input[name="selectedYears"]');

   yearCheckboxes.forEach(checkbox => {
      const conditional = document.getElementById(checkbox.getAttribute('data-aria-controls'));

      // Set initial state
      if (checkbox.checked) {
         conditional.classList.remove('govuk-checkboxes__conditional--hidden');
      }

      // Handle changes
      checkbox.addEventListener('change', (e) => {
         if (e.target.checked) {
            conditional.classList.remove('govuk-checkboxes__conditional--hidden');
         } else {
            conditional.classList.add('govuk-checkboxes__conditional--hidden');
         }
      });
   });

   yearCheckboxes.forEach(checkbox => {
      checkbox.addEventListener('change', function(e) {
         if (!e.target.checked) {
            const year = e.target.value;

            // Find and uncheck all month checkboxes for the unchecked year
            const monthCheckboxes = document.querySelectorAll(`input[name="selectedMonths"][value^="${year} "]`);
            monthCheckboxes.forEach(monthCheckbox => {
               monthCheckbox.checked = false;
            });
         }
      });
   });
});
