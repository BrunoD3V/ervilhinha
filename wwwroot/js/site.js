// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// ========================================
// ✨ ERVILHINHA - MODAL & BACKDROP FIX
// Garante hierarquia z-index correta
// ========================================

// Helper function to close modal and reset form
function closeModalAndReset(modalId) {
    const modalElement = document.getElementById(modalId);
    if (modalElement) {
        // Usar o método Bootstrap correto
        const modalInstance = bootstrap.Modal.getInstance(modalElement) || new bootstrap.Modal(modalElement);
        modalInstance.hide();

        // Reset form após um pequeno delay
        setTimeout(() => {
            const form = modalElement.querySelector('form');
            if (form) {
                form.reset();
            }
        }, 300);
    }
}

// ========================================
// 🔧 MODAL BACKDROP FIX - FORÇA DENTRO DO BODY
// ========================================
function ensureBackdropInBody() {
    // Verifica se há backdrops fora do body
    const bodyBackdrops = document.querySelectorAll('body > .modal-backdrop');
    const htmlBackdrops = document.querySelectorAll('html > .modal-backdrop');

    if (htmlBackdrops.length > 0) {
        console.warn('⚠️ BACKDROP FORA DO BODY DETECTADO! Movendo para dentro...');
        htmlBackdrops.forEach(backdrop => {
            document.body.appendChild(backdrop);
        });
    }

    // Força z-index correto em todos os backdrops
    bodyBackdrops.forEach(backdrop => {
        backdrop.style.zIndex = '1040';
    });

    // Garante que modais têm z-index superior
    document.querySelectorAll('.modal').forEach(modal => {
        if (modal.classList.contains('show')) {
            modal.style.zIndex = '1056';
        }
    });
}

// ========================================
// 🎯 MODAL EVENT LISTENERS - CONTROLO COMPLETO
// ========================================
function setupModalFixListeners() {
    document.querySelectorAll('.modal').forEach(function(modalEl) {

        // ANTES de mostrar
        modalEl.addEventListener('show.bs.modal', function (event) {
            console.log('🚀 Modal abrindo:', modalEl.id);

            // Garante body scrollable está correto
            document.body.style.overflow = 'hidden';
            document.body.style.paddingRight = '0px'; // Remove scroll width compensation
        });

        // DEPOIS de mostrar (modal já está visível)
        modalEl.addEventListener('shown.bs.modal', function (event) {
            console.log('✅ Modal aberto:', modalEl.id);

            // FORÇA backdrop para dentro do body
            ensureBackdropInBody();

            // Focus no primeiro input
            const firstInput = modalEl.querySelector('input:not([type="hidden"]), select, textarea');
            if (firstInput) {
                setTimeout(() => firstInput.focus(), 100);
            }
        });

        // ANTES de esconder
        modalEl.addEventListener('hide.bs.modal', function (event) {
            console.log('🔄 Modal fechando:', modalEl.id);
        });

        // DEPOIS de esconder
        modalEl.addEventListener('hidden.bs.modal', function (event) {
            console.log('✅ Modal fechado:', modalEl.id);

            // Limpa backdrops órfãos (bug conhecido do Bootstrap)
            document.querySelectorAll('.modal-backdrop').forEach(backdrop => {
                if (!backdrop.previousElementSibling || 
                    !backdrop.previousElementSibling.classList.contains('modal')) {
                    backdrop.remove();
                }
            });

            // Restaura scroll se não há mais modais abertos
            const openModals = document.querySelectorAll('.modal.show');
            if (openModals.length === 0) {
                document.body.style.overflow = '';
                document.body.style.paddingRight = '';
                document.body.classList.remove('modal-open');
            }
        });
    });
}

// ========================================
// 🎨 FLOATING LABEL ANIMATION FIX
// Garante que labels flutuam corretamente
// ========================================
function setupFloatingLabels() {
    document.querySelectorAll('.form-floating-modern input, .form-floating-modern select, .form-floating-modern textarea').forEach(input => {

        // Verifica valor inicial
        function checkValue() {
            if (input.value && input.value.trim() !== '') {
                input.classList.add('has-value');
            } else {
                input.classList.remove('has-value');
            }
        }

        // Eventos
        input.addEventListener('input', checkValue);
        input.addEventListener('change', checkValue);
        input.addEventListener('blur', checkValue);

        // Check inicial
        checkValue();
    });
}

// ========================================
// ⚡ HTMX EVENT LISTENERS - INTEGRAÇÃO
// ========================================
function setupHtmxIntegration() {
    if (typeof htmx !== 'undefined') {
        console.log('✅ HTMX detectado - configurando integração');

        // Quando HTMX carrega novo conteúdo
        document.body.addEventListener('htmx:afterSwap', function(event) {
            console.log('🔄 HTMX swap completo');

            // Re-setup floating labels em novo conteúdo
            setupFloatingLabels();

            // Re-setup modal listeners
            setupModalFixListeners();
        });

        // Indicador de loading suave
        document.body.addEventListener('htmx:beforeRequest', function(event) {
            const target = event.detail.target;
            target.classList.add('htmx-loading');
            target.style.opacity = '0.6';
            target.style.pointerEvents = 'none';
        });

        document.body.addEventListener('htmx:afterRequest', function(event) {
            const target = event.detail.target;
            target.classList.remove('htmx-loading');
            target.style.opacity = '';
            target.style.pointerEvents = '';
        });
    }
}

// ========================================
// 🎯 Z-INDEX MONITOR - DEBUGGING TOOL
// ========================================
function monitorZIndex() {
    // Só ativa em dev (comentar em produção)
    const isDev = window.location.hostname === 'localhost' || window.location.hostname === '127.0.0.1';

    if (isDev) {
        setInterval(() => {
            const modals = document.querySelectorAll('.modal.show');
            const backdrops = document.querySelectorAll('.modal-backdrop');

            if (modals.length > 0 || backdrops.length > 0) {
                console.log('📊 Z-Index Status:');
                modals.forEach((modal, i) => {
                    console.log(`  Modal ${i+1} (${modal.id}):`, window.getComputedStyle(modal).zIndex);
                });
                backdrops.forEach((backdrop, i) => {
                    const parent = backdrop.parentElement;
                    console.log(`  Backdrop ${i+1} parent:`, parent.tagName, 'z-index:', window.getComputedStyle(backdrop).zIndex);
                });
            }
        }, 5000); // Check a cada 5 segundos
    }
}

// ========================================
// 🚀 INITIALIZATION
// ========================================
document.addEventListener('DOMContentLoaded', function() {
    console.log('🌿 Ervilhinha - Initializing...');
    console.log('✅ Bootstrap version:', typeof bootstrap !== 'undefined' ? 'loaded' : '❌ NOT loaded');
    console.log('✅ HTMX version:', typeof htmx !== 'undefined' ? 'loaded' : '❌ NOT loaded');

    // Setup all features
    setupModalFixListeners();
    setupFloatingLabels();
    setupHtmxIntegration();
    monitorZIndex();

    // Test modal buttons
    const modalButtons = document.querySelectorAll('[data-bs-toggle="modal"]');
    console.log(`📋 Found ${modalButtons.length} modal trigger buttons`);

    // Verifica estrutura HTML
    const bodyChildren = document.body.children.length;
    const htmlChildren = document.documentElement.children.length;
    console.log(`📐 Body children: ${bodyChildren}, HTML children: ${htmlChildren}`);

    console.log('✨ Ervilhinha - Ready!');
});

// ========================================
// 🔧 GLOBAL ERROR HANDLER
// ========================================
window.addEventListener('error', function(event) {
    if (event.message.includes('backdrop') || event.message.includes('modal')) {
        console.error('❌ Modal/Backdrop Error:', event.message);
        ensureBackdropInBody();
    }
});

// ========================================
// 🎯 EXPOSE UTILITIES GLOBALLY
// ========================================
window.ErvilhinhaUtils = {
    closeModalAndReset,
    ensureBackdropInBody,
    setupFloatingLabels
};

console.log('🌿 Ervilhinha Utils loaded');
