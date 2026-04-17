document.addEventListener('DOMContentLoaded', async () => {
    try {
        const configResp = await fetch('config.json');
        const config = await configResp.json();
        
        const galleryResp = await fetch('gallery.json');
        const galleryList = await galleryResp.json();

        renderConfig(config);
        await renderGallery(galleryList);
        setupModal();
    } catch (err) {
        console.error('Failed to load configuration:', err);
    }
});

function renderConfig(config) {
    // Downloads
    const downloadList = document.getElementById('download-list');
    downloadList.innerHTML = '';
    
    config.downloads.forEach(dl => {
        const card = document.createElement('div');
        card.className = 'download-card';
        
        let iconClass = 'fas fa-download';
        if (dl.platform.toLowerCase().includes('android')) iconClass = 'fab fa-android';
        if (dl.platform.toLowerCase().includes('windows')) iconClass = 'fab fa-windows';
        if (dl.platform.toLowerCase().includes('ios') || dl.platform.toLowerCase().includes('apple')) iconClass = 'fab fa-apple';

        card.innerHTML = `
            <i class="${iconClass}"></i>
            <h3>${dl.platform}</h3>
            <a href="${dl.url}" class="btn btn-secondary">Download Now</a>
        `;
        downloadList.appendChild(card);
    });

    // Credits
    const creditsList = document.getElementById('credits-list');
    creditsList.innerHTML = '';
    config.credits.forEach(credit => {
        const card = document.createElement('div');
        card.className = 'credit-card';
        
        const nameContent = credit.url 
            ? `<a href="${credit.url}" target="_blank" rel="noopener noreferrer" class="credit-link">${credit.name}</a>`
            : credit.name;

        card.innerHTML = `
            <h4>${nameContent}</h4>
            <p>${credit.role}</p>
        `;
        creditsList.appendChild(card);
    });

    // Disclaimer
    const disclaimer = document.getElementById('disclaimer-text');
    disclaimer.textContent = config.disclaimer;

    // Dashboard Link
    const dashboardNav = document.getElementById('dashboard-nav');
    if (dashboardNav && config.dashboard_url) {
        dashboardNav.href = config.dashboard_url;
    }
}

async function renderGallery(items) {
    const grid = document.getElementById('gallery-grid');
    grid.innerHTML = '';

    for (const item of items) {
        let description = "Minions Paradise";
        let author = "Anonymous";
        let date = "Unknown Date";

        try {
            const descResp = await fetch(item.descFile);
            const text = await descResp.text();
            const lines = text.split('\n').map(l => l.trim()).filter(l => l.length > 0);
            
            if (lines.length >= 1) {
                description = lines[0].replace(/^TEXT\s*:\s*/i, '').trim();
            }
            if (lines.length >= 2) author = lines[1];
            if (lines.length >= 3) date = lines[2];
        } catch (e) {
            console.warn(`Could not load description for ${item.image}`);
        }

        const div = document.createElement('div');
        div.className = 'gallery-item fade-in';
        div.innerHTML = `
            <img src="${item.image}" alt="${description}">
            <div class="gallery-desc">${description}</div>
            <div class="passport-stamp">
                <span class="author">${author}</span>
                <span class="date">${date}</span>
            </div>
        `;

        div.addEventListener('click', () => {
            openModal(item.image);
        });

        grid.appendChild(div);
    }
}

function setupModal() {
    const modal = document.getElementById('image-modal');
    const closeBtn = document.querySelector('.modal-close');

    closeBtn.onclick = () => {
        modal.style.display = "none";
    };

    modal.onclick = (e) => {
        if (e.target === modal) {
            modal.style.display = "none";
        }
    };

    // Close on escape key
    document.addEventListener('keydown', (e) => {
        if (e.key === "Escape") {
            modal.style.display = "none";
        }
    });
}

function openModal(imgSrc) {
    const modal = document.getElementById('image-modal');
    const modalImg = document.getElementById('modal-img');
    modal.style.display = "flex";
    modalImg.src = imgSrc;
}
