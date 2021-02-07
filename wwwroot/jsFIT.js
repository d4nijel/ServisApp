function DodajAjaxEvente() {
    $("button[ajax-poziv='da']").click(function (event) {
        $(this).attr("ajax-poziv", "dodan");
        event.preventDefault();

        var urlZaPoziv = $(this).attr("ajax-url");
        var divZaRezultat = $(this).attr("ajax-rezultat");

        $.get(urlZaPoziv, function (data, status) {
            $("#" + divZaRezultat).html(data);
        });
    });

    $("a[ajax-poziv='da']").click(function (event) {
        $(this).attr("ajax-poziv", "dodan");
        event.preventDefault();

        var urlZaPoziv1 = $(this).attr("ajax-url");
        var urlZaPoziv2 = $(this).attr("href");
        var divZaRezultat = $(this).attr("ajax-rezultat");

        var urlZaPoziv;

        if (urlZaPoziv1 instanceof String)
            urlZaPoziv = urlZaPoziv1;
        else
            urlZaPoziv = urlZaPoziv2;

        $.get(urlZaPoziv, function (data, status) {
            $("#" + divZaRezultat).html(data);
        });
    });

    $("form[ajax-poziv='da']").submit(function (event) {
        $(this).attr("ajax-poziv", "dodan");

        event.preventDefault();
        event.stopImmediatePropagation();

        var forma = $(this);

        forma.validate();

        if (forma.valid()) {

            var urlZaPoziv1 = $(this).attr("ajax-url");
            var urlZaPoziv2 = $(this).attr("action");

            var divZaRezultat = $(this).attr("ajax-rezultat");

            var urlZaPoziv;
            if (urlZaPoziv1 instanceof String)
                urlZaPoziv = urlZaPoziv1;
            else
                urlZaPoziv = urlZaPoziv2;

            if (forma.attr("enctype") == "multipart/form-data") {

                var myForm = forma[0];
                var formData = new FormData(myForm);

                $.ajax({
                    url: urlZaPoziv,
                    data: formData,
                    contentType: false,
                    processData: false,
                    type: "POST",
                    success: function (data) {
                        $("#" + "ajaxPoziv").html(data);
                    }
                });
            }
            else {
                $.ajax({
                    type: "POST",
                    url: urlZaPoziv,
                    data: forma.serialize(),
                    success: function (data) {
                        $("#" + divZaRezultat).html(data);
                    }
                });
            }
        }
    });
}
$(document).ready(function () {
    // izvršava nakon što glavni html dokument bude generisan
    DodajAjaxEvente();
});

$(document).ajaxComplete(function () {
    // izvršava nakon bilo kojeg ajax poziva
    DodajAjaxEvente();
});
