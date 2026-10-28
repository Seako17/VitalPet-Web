function togglePasswordIcon(inputId, buttonId) {
    var passwordField = document.getElementById(inputId);
    var eyeBtn = document.getElementById(buttonId);

    if (passwordField && eyeBtn) {
        if (passwordField.type === "password") {
            passwordField.type = "text";
            eyeBtn.classList.add("eye-open");
        } else {
            passwordField.type = "password";
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

function toggleDropdown(event) {
    if (event) {
        event.preventDefault();
        event.stopPropagation();
    }
    var dropdown = event.currentTarget ? event.currentTarget.parentElement : null;
    if (!dropdown) {
        dropdown = event.target ? event.target.closest('.dropdown') : null;
    }
    if (!dropdown) {
        return;
    }
    dropdown.classList.toggle('open');
}

document.addEventListener('click', function (e) {
    var dropdowns = document.querySelectorAll('.dropdown');
    dropdowns.forEach(function (dropdown) {
        if (!dropdown.contains(e.target)) {
            dropdown.classList.remove('open');
        }
    });
});
