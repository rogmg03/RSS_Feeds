// =========================
// FOLLOW MODAL (Discover)
// =========================
(() => {
    let selectedFeedId = null;

    function openModal(feedId, feedTitle) {
        selectedFeedId = feedId;

        const modal = document.getElementById("followModal");
        const title = document.getElementById("followFeedTitle");
        const alias = document.getElementById("feedAlias");

        if (!modal || !title || !alias) {
            console.error("Follow modal elements not found");
            return;
        }

        title.textContent = feedTitle || "";
        alias.value = "";
        modal.classList.remove("hidden");
        alias.focus();
    }

    function closeModal() {
        const modal = document.getElementById("followModal");
        if (modal) modal.classList.add("hidden");
        selectedFeedId = null;
    }

    async function confirmFollow() {
        if (!selectedFeedId) return;

        const alias = document.getElementById("feedAlias")?.value?.trim() || null;

        try {
            const res = await fetch("/Feeds/Follow", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({
                    feedId: Number(selectedFeedId),
                    alias: alias
                })
            });

            if (!res.ok) throw new Error();

            location.reload();
        } catch {
            alert("Error al seguir el feed");
        }
    }

    // -------------------------
    // EVENT DELEGATION
    // -------------------------
    document.addEventListener("click", (e) => {

        // Abrir modal
        const openBtn = e.target.closest(".js-follow-open");
        if (openBtn) {
            openModal(
                openBtn.dataset.feedId,
                openBtn.dataset.feedTitle
            );
            return;
        }

        // Confirmar follow
        if (e.target.id === "confirmFollowBtn") {
            confirmFollow();
            return;
        }

        // Cancelar
        if (e.target.id === "cancelFollowBtn") {
            closeModal();
            return;
        }

        // Click fuera del modal
        if (e.target.id === "followModal") {
            closeModal();
        }
    });

    // ESC para cerrar
    document.addEventListener("keydown", (e) => {
        if (e.key === "Escape") closeModal();
    });
})();


// =========================
// ADD FEED MODAL (Discover)
// =========================
(() => {
    function openAddFeedModal() {
        const modal = document.getElementById("addFeedModal");
        const urlInput = document.getElementById("feedUrl");
        const titleInput = document.getElementById("feedTitle");
        const descriptionInput = document.getElementById("feedDescription");
        const categoryInput = document.getElementById("feedCategory");
        const languageInput = document.getElementById("feedLanguage");
        const errorDiv = document.getElementById("addFeedError");

        if (!modal || !urlInput || !titleInput || !descriptionInput || !categoryInput || !languageInput || !errorDiv) {
            console.error("Add feed modal elements not found");
            return;
        }

        // Limpiar campos
        urlInput.value = "";
        titleInput.value = "";
        descriptionInput.value = "";
        categoryInput.value = "";
        languageInput.value = "";
        errorDiv.style.display = "none";
        errorDiv.textContent = "";

        modal.classList.remove("hidden");
        urlInput.focus();
    }

    function closeAddFeedModal() {
        const modal = document.getElementById("addFeedModal");
        if (modal) modal.classList.add("hidden");
    }

    function showError(message) {
        const errorDiv = document.getElementById("addFeedError");
        if (errorDiv) {
            errorDiv.textContent = message;
            errorDiv.style.display = "block";
        }
    }

    function hideError() {
        const errorDiv = document.getElementById("addFeedError");
        if (errorDiv) {
            errorDiv.style.display = "none";
            errorDiv.textContent = "";
        }
    }

    async function confirmAddFeed() {
        const urlInput = document.getElementById("feedUrl");
        const titleInput = document.getElementById("feedTitle");
        const descriptionInput = document.getElementById("feedDescription");
        const categoryInput = document.getElementById("feedCategory");
        const languageInput = document.getElementById("feedLanguage");

        if (!urlInput || !titleInput || !descriptionInput || !categoryInput || !languageInput) {
            return;
        }

        const url = urlInput.value?.trim();
        const title = titleInput.value?.trim() || null;
        const description = descriptionInput.value?.trim() || null;
        const category = categoryInput.value?.trim() || null;
        const language = languageInput.value?.trim() || null;

        // Validar URL
        if (!url) {
            showError("La URL del feed es requerida");
            urlInput.focus();
            return;
        }

        hideError();

        const payload = {
            url: url,
            titulo: title,
            descripcion: description,
            categoria: category,
            idioma: language
        };

        try {
            const res = await fetch("/Feeds/CreateFeed", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(payload)
            });

            if (!res.ok) {
                const errorText = await res.text();
                showError(errorText || "Error al crear el feed. Puede que ya exista un feed con esta URL.");
                return;
            }

            // Éxito: recargar página
            location.reload();
        } catch (error) {
            showError("Error al conectar con el servidor. Por favor, intenta nuevamente.");
        }
    }

    // -------------------------
    // EVENT DELEGATION
    // -------------------------
    document.addEventListener("click", (e) => {
        // Abrir modal
        const openBtn = e.target.closest(".js-add-feed-open");
        if (openBtn) {
            openAddFeedModal();
            return;
        }

        // Confirmar agregar feed
        if (e.target.id === "confirmAddFeedBtn") {
            confirmAddFeed();
            return;
        }

        // Cancelar
        if (e.target.id === "cancelAddFeedBtn") {
            closeAddFeedModal();
            return;
        }

        // Click fuera del modal
        if (e.target.id === "addFeedModal") {
            closeAddFeedModal();
        }
    });

    // ESC para cerrar
    document.addEventListener("keydown", (e) => {
        const modal = document.getElementById("addFeedModal");
        if (modal && !modal.classList.contains("hidden") && e.key === "Escape") {
            closeAddFeedModal();
        }
    });
})();


// =========================
// LIKE / UNLIKE (FeedCard)
// =========================
document.addEventListener("click", async (e) => {
    const btn = e.target.closest(".like-btn");
    if (!btn) return;

    const payload = {
        feedId: Number(btn.dataset.feedId),
        titulo: btn.dataset.titulo,
        link: btn.dataset.link,
        descripcion: btn.dataset.descripcion,
        contenido: btn.dataset.contenido || null,
        autor: btn.dataset.autor || null,
        imagen: btn.dataset.imagen || null,
        fechaPublicacion: btn.dataset.fecha || null
    };

    btn.disabled = true;

    try {
        const res = await fetch("/Articles/Like", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(payload)
        });

        if (!res.ok) throw new Error();

        btn.classList.add("liked");
        btn.textContent = "❤️";
    } catch {
        alert("No se pudo guardar el artículo");
    } finally {
        btn.disabled = false;
    }
});


// =========================
// RSS FINDER SEARCH (Discover)
// =========================
(() => {
    let searchTimeout = null;
    const existingFeeds = new Map(); // URL -> FeedDTO

    // Cargar feeds existentes al iniciar
    async function loadExistingFeeds() {
        try {
            // Obtener feeds directamente desde la API
            const res = await fetch("/api/ServiceLocator/feeds");
            if (res.ok) {
                const feeds = await res.json();
                if (feeds && Array.isArray(feeds)) {
                    feeds.forEach(feed => {
                        if (feed.url) {
                            existingFeeds.set(feed.url, feed);
                        }
                    });
                }
            }
        } catch (error) {
            console.error("Error loading existing feeds:", error);
        }
    }

    // Debounce para búsqueda
    function debounceSearch(keyword) {
        clearTimeout(searchTimeout);
        searchTimeout = setTimeout(() => {
            performSearch(keyword);
        }, 500);
    }

    // Realizar búsqueda
    async function performSearch(keyword) {
        if (!keyword || keyword.trim().length < 3) {
            hideResults();
            return;
        }

        const resultsContainer = document.getElementById("rssFinderResults");
        const loadingDiv = document.getElementById("rssFinderLoading");
        const errorDiv = document.getElementById("rssFinderError");
        const feedGrid = document.getElementById("rssFinderFeedGrid");

        if (!resultsContainer || !loadingDiv || !errorDiv || !feedGrid) return;

        // Mostrar loading
        resultsContainer.classList.remove("hidden");
        loadingDiv.style.display = "block";
        errorDiv.style.display = "none";
        feedGrid.innerHTML = "";

        try {
            const res = await fetch(`/Feeds/SearchRssFinder?keyword=${encodeURIComponent(keyword)}`);

            loadingDiv.style.display = "none";

            if (!res.ok) {
                const errorText = await res.text();
                showError(errorText || "Error al buscar feeds");
                return;
            }

            const feeds = await res.json();

            if (!feeds || feeds.length === 0) {
                showError(`No se encontraron feeds para "${keyword}"`);
                return;
            }

            // Renderizar resultados
            renderFeeds(feeds);
        } catch (error) {
            loadingDiv.style.display = "none";
            showError("Error al conectar con el servidor. Por favor, intenta nuevamente.");
        }
    }

    // Renderizar feeds encontrados
    function renderFeeds(feeds) {
        const feedGrid = document.getElementById("rssFinderFeedGrid");
        if (!feedGrid) return;

        feedGrid.innerHTML = "";

        feeds.forEach(feed => {
            const feedExists = existingFeeds.has(feed.url);
            const feedCard = createFeedCard(feed, feedExists);
            feedGrid.appendChild(feedCard);
        });
    }

    // Crear card de feed
    function createFeedCard(feed, existsInDb) {
        const card = document.createElement("article");
        card.className = "feed-discover-card rss-finder-card";

        const avatar = feed.titulo && feed.titulo.length >= 2
            ? feed.titulo.substring(0, 2).toUpperCase()
            : "FD";

        card.innerHTML = `
            <div class="feed-discover-header">
                <div class="feed-avatar">${avatar}</div>
                <div class="feed-info">
                    <div class="feed-name">${escapeHtml(feed.titulo || feed.url)}</div>
                    <div class="feed-url">${escapeHtml(feed.url)}</div>
                </div>
            </div>
            ${feed.descripcion ? `<p class="feed-description">${escapeHtml(feed.descripcion)}</p>` : ""}
            <button type="button"
                    class="btn-follow ${existsInDb ? "following" : ""} js-rss-finder-add"
                    data-feed-url="${escapeHtml(feed.url)}"
                    data-feed-title="${escapeHtml(feed.titulo || "")}"
                    data-feed-description="${escapeHtml(feed.descripcion || "")}"
                    ${existsInDb ? "disabled" : ""}>
                ${existsInDb ? "Ya existe" : "Agregar"}
            </button>
        `;

        return card;
    }

    // Escapar HTML
    function escapeHtml(text) {
        if (!text) return "";
        const div = document.createElement("div");
        div.textContent = text;
        return div.innerHTML;
    }

    // Mostrar error
    function showError(message) {
        const errorDiv = document.getElementById("rssFinderError");
        if (errorDiv) {
            errorDiv.textContent = message;
            errorDiv.style.display = "block";
        }
    }

    // Ocultar resultados
    function hideResults() {
        const resultsContainer = document.getElementById("rssFinderResults");
        if (resultsContainer) {
            resultsContainer.classList.add("hidden");
        }
    }

    // Agregar feed desde resultados
    async function addFeedFromResults(feedUrl, feedTitle, feedDescription) {
        const payload = {
            url: feedUrl,
            titulo: feedTitle || null,
            descripcion: feedDescription || null,
            categoria: null,
            idioma: null
        };

        try {
            const res = await fetch("/Feeds/CreateFeed", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(payload)
            });

            if (!res.ok) {
                const errorText = await res.text();
                alert(errorText || "Error al agregar el feed");
                return false;
            }

            // Agregar a mapa de feeds existentes
            existingFeeds.set(feedUrl, payload);

            // Actualizar UI
            const buttons = document.querySelectorAll(`.js-rss-finder-add[data-feed-url="${feedUrl}"]`);
            buttons.forEach(btn => {
                btn.textContent = "Ya existe";
                btn.classList.add("following");
                btn.disabled = true;
            });

            return true;
        } catch (error) {
            alert("Error al agregar el feed. Por favor, intenta nuevamente.");
            return false;
        }
    }

    // Event listeners
    const searchInput = document.getElementById("feedSearchInput");
    if (searchInput) {
        searchInput.addEventListener("input", (e) => {
            const keyword = e.target.value.trim();
            if (keyword.length >= 3) {
                debounceSearch(keyword);
            } else {
                hideResults();
            }
        });
    }

    // Limpiar búsqueda
    const clearBtn = document.getElementById("clearSearchResults");
    if (clearBtn) {
        clearBtn.addEventListener("click", () => {
            if (searchInput) searchInput.value = "";
            hideResults();
        });
    }

    // Agregar feed desde resultados
    document.addEventListener("click", async (e) => {
        const btn = e.target.closest(".js-rss-finder-add");
        if (!btn || btn.disabled) return;

        const feedUrl = btn.dataset.feedUrl;
        const feedTitle = btn.dataset.feedTitle;
        const feedDescription = btn.dataset.feedDescription;

        if (!feedUrl) return;

        btn.disabled = true;
        btn.textContent = "Agregando...";

        const success = await addFeedFromResults(feedUrl, feedTitle, feedDescription);

        if (!success) {
            btn.disabled = false;
            btn.textContent = "Agregar";
        }
    });

    // Cargar feeds existentes al cargar la página
    if (document.readyState === "loading") {
        document.addEventListener("DOMContentLoaded", loadExistingFeeds);
    } else {
        loadExistingFeeds();
    }
})();