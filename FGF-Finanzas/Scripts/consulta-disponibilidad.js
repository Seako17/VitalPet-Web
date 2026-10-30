(function () {
    'use strict';

    function format(template, value) {
        return String(template || '').replace('{0}', value);
    }

    function init() {
        var panel = document.getElementById('pnlDisponibilidad');
        if (!panel) return;

        var mascota = document.getElementById('dropMascota');
        var fecha = document.getElementById('txtFecha');
        var veterinario = document.getElementById('ddlVeterinario');
        var hora = document.getElementById('ddlHora');

        if (!mascota || !fecha || !veterinario || !hora) return;

        var url = panel.getAttribute('data-url');
        var msgCantidad = panel.getAttribute('data-msg-cantidad');
        var msgPrimero = panel.getAttribute('data-msg-primero');
        var msgSin = panel.getAttribute('data-msg-sin');
        var msgConsultando = panel.getAttribute('data-msg-consultando');
        var msgSesion = panel.getAttribute('data-msg-sesion');
        var msgError = panel.getAttribute('data-msg-error');
        var mensajeEl = panel.querySelector('.disponibilidad-mensaje');

        var peticionActual = null;
        var ultimaClave = '';

        function setEstado(clase, texto) {
            if (!mensajeEl) return;
            mensajeEl.className = 'disponibilidad-mensaje ' + (clase || '');
            mensajeEl.textContent = texto || '';
        }

        function limpiarHoras() {
            var options = hora.options;
            for (var i = 0; i < options.length; i++) {
                options[i].disabled = false;
                options[i].removeAttribute('title');
            }
        }

        function aplicarHoras(horariosLibres) {
            var setLibres = {};
            for (var i = 0; i < horariosLibres.length; i++) {
                setLibres[horariosLibres[i]] = true;
            }

            var options = hora.options;
            var seleccionActualValida = false;

            for (var j = 0; j < options.length; j++) {
                var val = options[j].value;
                if (!val) continue; // placeholder "Hora"
                options[j].disabled = false;
                options[j].removeAttribute('title');
                if (!setLibres[val]) {
                    options[j].disabled = true;
                } else {
                    if (options[j].selected) {
                        seleccionActualValida = true;
                    }
                }
            }

            if (!seleccionActualValida && hora.value) {
                hora.value = "";
            }
        }

        function claveActual() {
            return mascota.value + '|' + fecha.value + '|' + veterinario.value;
        }

        function refrescarDisponibilidad(forzar) {
            if (!mascota.value || !fecha.value || !veterinario.value) {
                limpiarHoras();
                setEstado('', '');
                ultimaClave = '';
                return;
            }

            var k = claveActual();
            if (!forzar && k === ultimaClave) return;
            ultimaClave = k;

            setEstado('is-loading', msgConsultando);

            if (peticionActual && typeof peticionActual.abort === 'function') {
                peticionActual.abort();
            }

            var controller = window.AbortController ? new AbortController() : null;
            peticionActual = controller;

            var payload = {
                idMascota: parseInt(mascota.value, 10),
                dniVeterinario: veterinario.value,
                fecha: fecha.value
            };

            fetch(url, {
                method: 'POST',
                credentials: 'same-origin',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8'
                },
                body: JSON.stringify(payload),
                signal: controller ? controller.signal : undefined
            })
            .then(function (response) {
                return response.json();
            })
            .then(function (data) {
                if (controller && controller.signal.aborted) return;
                var res = data && data.d;
                if (!res) {
                    setEstado('is-error', msgError);
                    limpiarHoras();
                    return;
                }

                if (res.Ok) {
                    var libres = res.HorariosLibres || [];
                    aplicarHoras(libres);

                    if (res.CantidadHorarios > 0) {
                        var texto = format(msgCantidad, res.CantidadHorarios);
                        if (res.PrimerHorarioDisponible) {
                            texto += ' · ' + format(msgPrimero, res.PrimerHorarioDisponible);
                        }
                        setEstado('is-ok', texto);
                    } else {
                        setEstado('is-empty', msgSin);
                    }
                } else {
                    limpiarHoras();
                    if (res.CodigoError === 'ERR_SESION') {
                        setEstado('is-error', msgSesion || res.Mensaje);
                    } else {
                        setEstado('is-error', res.Mensaje || msgError);
                    }
                }
            })
            .catch(function (err) {
                if (err && err.name === 'AbortError') return;
                setEstado('is-error', msgError);
                limpiarHoras();
            });
        }

        mascota.addEventListener('change', function () { refrescarDisponibilidad(true); });
        fecha.addEventListener('change', function () { refrescarDisponibilidad(true); });
        veterinario.addEventListener('change', function () { refrescarDisponibilidad(true); });

        // Si ya vienen seleccionados (ej: tras recarga o postback)
        refrescarDisponibilidad(true);
    }

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', init);
    } else {
        init();
    }
})();
