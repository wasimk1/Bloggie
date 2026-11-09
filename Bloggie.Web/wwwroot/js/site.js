// Presentation-only image fallback. Navigation and form submissions remain native.
document.querySelectorAll('[data-image-fallback]').forEach(function (image) {
    function showFallback() { image.hidden = true; }
    image.addEventListener('error', showFallback);
    if (image.complete && image.naturalWidth === 0) showFallback();
});
