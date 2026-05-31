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