function togglePasswordIcon() {
    var eyeBtn = document.getElementById('togglePassword');

    var passwordField = document.getElementById('Password');
    var confirmField = document.getElementById('ConfirmPassword');

    if (passwordField && eyeBtn) {
        if (passwordField.type === "password") {
            passwordField.type = "text";
            if (confirmField) {
                confirmField.type = "text";
            }
            eyeBtn.classList.add("eye-open");
        } else {
            passwordField.type = "password";
            if (confirmField) {
                confirmField.type = "password";
            }
            eyeBtn.classList.remove("eye-open");
        }
    }

}

document.addEventListener("DOMContentLoaded", function () {
    const mobileMenu = document.getElementById("mobile-menu");
    const navMenu = document.getElementById("nav-menu");
    if (mobileMenu && navMenu) {
        mobileMenu.addEventListener("click", function () {
            navMenu.classList.toggle("active");
            mobileMenu.classList.toggle("active");
        });
    }
});

function updateFileName(input) {
    var fileName = input.files[0] ? input.files[0].name : "";
    document.getElementById("fileNameLabel").innerText = fileName;
}

function mostrarAlerta(mensaje, tipo) {
    var contenedor = document.getElementById('contenedor-alertas');
    if (contenedor) {
        var nuevaAlerta = document.createElement('div');
        nuevaAlerta.className = tipo === 'error' ? 'alerta-web alerta-error' : 'alerta-web';
        nuevaAlerta.innerText = mensaje;

        contenedor.appendChild(nuevaAlerta);

        setTimeout(function () {
            nuevaAlerta.remove();
        }, 5000);
    }
}