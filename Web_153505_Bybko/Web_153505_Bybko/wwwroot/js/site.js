document.querySelectorAll('.page-link').forEach(function (element) {
    element.addEventListener('click', function (e) {
        e.preventDefault();

        // definding URL for request
        var url = element.getAttribute('href');

        // sending AJAX-request
        fetch(url, {
            method: 'GET'
        })
    });
});