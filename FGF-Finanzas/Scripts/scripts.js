function mostrarContraseña(chk) {
    var inputs = document.querySelectorAll('input[id*="Password"]');
    var tipo = chk.checked ? "text" : "password";

    inputs.forEach(function (input) {
        input.type = tipo;
    });
}
