function mostrarContraseña(chk) {
    var pass = document.querySelector('input[id*="Password"]');
    var confirm = document.querySelector('input[id*="ConfirmPassword"]');

    if (pass && confirm) {
        var tipo = chk.checked ? "text" : "password";
        pass.type = tipo;
        confirm.type = tipo;
    }
}