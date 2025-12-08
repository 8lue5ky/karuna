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

// pull-to-refresh.js
window.registerPullToRefresh = function (dotnetRef) {
    let startY = 0;
    let currentY = 0;
    let isPulling = false;
    const threshold = 70; // ab welcher Distanz ein Refresh ausgelöst wird

    window.addEventListener("touchstart", e => {
        if (window.pageYOffset === 0) {
            startY = e.touches[0].clientY;
            isPulling = true;
        }
    });

    window.addEventListener("touchmove", e => {
        if (!isPulling) return;

        currentY = e.touches[0].clientY;

        if (currentY - startY > threshold) {
            // Pull-to-refresh erkannt
            dotnetRef.invokeMethodAsync("OnPullToRefresh");
            isPulling = false;
        }
    });

    window.addEventListener("touchend", () => {
        isPulling = false;
    });
};
