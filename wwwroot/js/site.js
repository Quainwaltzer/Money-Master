(function () {
    window.chatUI = chatContainer;

    function chatContainer(element) {
        element.classList.remove("popup");

        void element.offsetWidth;

        element.classList.add("popup");
    }
})();