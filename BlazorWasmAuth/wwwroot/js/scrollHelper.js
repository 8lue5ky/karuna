// globaler Speicher für die .NET-Referenz
window.blazorScrollRef = null;

// registriert den Scroll-Listener auf window
window.registerScrollHandler = function (dotnetRef) {
    window.blazorScrollRef = dotnetRef;

    window.addEventListener("scroll", function () {
        if (window.blazorScrollRef) {
            window.blazorScrollRef.invokeMethodAsync("OnWindowScroll");
        }
    });
};

// gibt Scroll-Informationen zurück
window.getScrollInfo = function () {
    return {
        scrollTop: window.pageYOffset,
        windowHeight: window.innerHeight,
        scrollHeight: document.documentElement.scrollHeight
    };
};