document.addEventListener('DOMContentLoaded', function() {
  const changeLinks = document.querySelectorAll('.change-link');

  changeLinks.forEach(link => {
    link.addEventListener('click', function(event) {
      event.preventDefault();

      const currentUrl = window.location.pathname;
      const form = document.createElement('form');
      form.method = 'POST';
      form.action = `${currentUrl}?handler=ChangeLink`;

      const fields = {
        'Id': this.getAttribute('data-id'),
        'NextPage': this.getAttribute('data-next-page'),
        '__RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
      };

      Object.entries(fields).forEach(([name, value]) => {
        const input = document.createElement('input');
        input.type = 'hidden';
        input.name = name;
        input.value = value || '';
        form.appendChild(input);
      });

      document.body.appendChild(form);
      form.submit();
      document.body.removeChild(form);
    });
  });
});