const currentPath = window.location.pathname.toLowerCase();
document.querySelectorAll(".navbar-nav .nav-link").forEach(function (link) {
    const href = link.getAttribute("href").toLowerCase();
    if (href !== "/" && currentPath.startsWith(href)) {
        link.classList.add("active");
    }
});


const search = document.getElementById("tableSearch");
const typeFilter = document.getElementById("typeFilter");
const rows = document.querySelectorAll("table tbody tr");

function filterRows() {
    const text = search ? search.value.toLowerCase() : "";
    const type = typeFilter ? typeFilter.value : "";

    rows.forEach(function (row) {
        const matchText = row.innerText.toLowerCase().includes(text);
        const typeCell = row.querySelector(".ship-type");
        const matchType = !type || (typeCell && typeCell.innerText === type);
        row.style.display = matchText && matchType ? "" : "none";
    });
}

if (search) {
    search.addEventListener("input", filterRows);
}

if (typeFilter) {
    const types = new Set();
    document.querySelectorAll(".ship-type").forEach(function (cell) {
        types.add(cell.innerText);
    });
    types.forEach(function (t) {
        const option = document.createElement("option");
        option.value = t;
        option.textContent = t;
        typeFilter.appendChild(option);
    });
    typeFilter.addEventListener("change", filterRows);
}


const fileInput = document.getElementById("Files");
const preview = document.getElementById("preview");

if (fileInput && preview) {
    fileInput.addEventListener("change", function () {
        preview.innerHTML = "";
        Array.from(fileInput.files).forEach(function (file) {
            const col = document.createElement("div");
            col.className = "col-md-4 mb-2";
            const img = document.createElement("img");
            img.src = URL.createObjectURL(file);
            img.className = "ship-img";
            col.appendChild(img);
            preview.appendChild(col);
        });
    });
}