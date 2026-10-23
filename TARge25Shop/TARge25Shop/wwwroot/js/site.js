document.addEventListener("DOMContentLoaded", function () {

    var path = window.location.pathname.toLowerCase();
    document.querySelectorAll(".site-navbar .nav-link").forEach(function (link) {
        var href = link.getAttribute("href").toLowerCase();
        var isHome = href === "/" && path === "/";
        var isSection = href !== "/" && path.startsWith(href.replace("/index", ""));
        if (isHome || isSection) {
            link.classList.add("active");
        }
    });


    var searchInput = document.getElementById("searchInput");
    if (searchInput) {
        searchInput.addEventListener("input", function () {
            var text = searchInput.value.toLowerCase();
            var found = 0;

            document.querySelectorAll(".kg-item").forEach(function (card) {
                var match = card.innerText.toLowerCase().includes(text);
                card.style.display = match ? "" : "none";
                if (match) { found++; }
            });

            var noResults = document.getElementById("noResults");
            if (noResults) {
                noResults.style.display = found === 0 ? "block" : "none";
            }
        });
    }


    var fileInput = document.getElementById("fileInput");
    var previewBox = document.getElementById("previewBox");
    if (fileInput && previewBox) {
        fileInput.addEventListener("change", function () {
            previewBox.innerHTML = "";
            Array.from(fileInput.files).forEach(function (file) {
                var img = document.createElement("img");
                img.className = "gallery-img";
                img.src = URL.createObjectURL(file);
                previewBox.appendChild(img);
            });
        });
    }


    document.addEventListener("click", function (e) {
        if (e.target.classList.contains("gallery-img")) {
            var box = document.createElement("div");
            box.className = "lightbox";
            box.innerHTML = '<img src="' + e.target.src + '" alt="" />';
            box.addEventListener("click", function () { box.remove(); });
            document.body.appendChild(box);
        }
    });
});