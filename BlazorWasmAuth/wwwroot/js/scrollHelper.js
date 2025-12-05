// registriert den Scroll-Listener auf window
window.registerScrollHandler = function (dotnetRef) {
    window.addEventListener("scroll", () => {
        dotnetRef.invokeMethodAsync("OnWindowScroll", {
            scrollTop: window.pageYOffset,
            windowHeight: window.innerHeight,
            scrollHeight: document.documentElement.scrollHeight
        });
    });
};