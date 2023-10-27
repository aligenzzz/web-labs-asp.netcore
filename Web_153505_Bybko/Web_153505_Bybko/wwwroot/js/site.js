$(document).ready(function () {
    $('.page-link').on('click', function (e) {
        e.preventDefault();
        let url = this.attributes['href'].value;

        $('#bookList').load(url);
    })
})