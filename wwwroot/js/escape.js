// Funcionalidades del lado del cliente para "Escapar del Chacho".
// Las validaciones importantes (si la clave es correcta, si se puede
// acceder a una sala, etc.) se hacen siempre en el servidor. Esto es
// solo para mejorar la experiencia del jugador en el navegador.

document.addEventListener("DOMContentLoaded", function () {

    // Al abrir el modal de pista, se pone el foco en el botón de cerrar
    // para que el jugador pueda volver rápido al campo de respuesta.
    var modalPista = document.getElementById("modalPista");
    if (modalPista) {
        modalPista.addEventListener("shown.bs.modal", function () {
            var botonCerrar = modalPista.querySelector(".btn-comenzar");
            if (botonCerrar) {
                botonCerrar.focus();
            }
        });

        // Al cerrar el modal, se devuelve el foco al campo de respuesta
        // para que el jugador pueda seguir escribiendo sin perder nada.
        modalPista.addEventListener("hidden.bs.modal", function () {
            var campoRespuesta = document.getElementById("respuesta");
            if (campoRespuesta) {
                campoRespuesta.focus();
            }
        });
    }

});
