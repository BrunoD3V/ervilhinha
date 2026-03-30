// ========================================
// 🎆 ERVILHINHA - EFEITOS ÉPICOS JS
// Interações avançadas para formulários
// ========================================

const ErvilhinhaEpic = {
    
    // ========================================
    // 🎊 CONFETTI EXPLOSION
    // ========================================
    confetti: {
        launch: function(duration = 3000) {
            const container = document.createElement('div');
            container.className = 'confetti-container';
            document.body.appendChild(container);
            
            const colors = ['#A5D6A7', '#F8BBD0', '#BBDEFB', '#FFCCBC', '#FFF9C4'];
            const pieceCount = 50;
            
            for (let i = 0; i < pieceCount; i++) {
                const piece = document.createElement('div');
                piece.className = 'confetti-piece';
                piece.style.left = Math.random() * 100 + '%';
                piece.style.backgroundColor = colors[Math.floor(Math.random() * colors.length)];
                piece.style.animationDelay = Math.random() * 0.5 + 's';
                piece.style.animationDuration = (Math.random() * 2 + 2) + 's';
                container.appendChild(piece);
            }
            
            setTimeout(() => {
                container.remove();
            }, duration);
        }
    },
    
    // ========================================
    // 💫 CURSOR TRAIL
    // ========================================
    cursorTrail: {
        enabled: false,
        
        enable: function() {
            if (this.enabled) return;
            this.enabled = true;
            
            document.addEventListener('mousemove', this.handleMouseMove.bind(this));
        },
        
        disable: function() {
            this.enabled = false;
            document.removeEventListener('mousemove', this.handleMouseMove.bind(this));
        },
        
        handleMouseMove: function(e) {
            if (!this.enabled) return;
            
            const trail = document.createElement('div');
            trail.className = 'cursor-trail';
            trail.style.left = e.pageX + 'px';
            trail.style.top = e.pageY + 'px';
            document.body.appendChild(trail);
            
            setTimeout(() => trail.remove(), 600);
        }
    },
    
    // ========================================
    // 🎨 FLOATING PARTICLES
    // ========================================
    particles: {
        add: function(cardElement) {
            if (!cardElement) return;
            
            cardElement.classList.add('with-particles');
            
            const emojis = ['✨', '💚', '👶', '🌿', '💫', '⭐'];
            const particleCount = 3;
            
            for (let i = 0; i < particleCount; i++) {
                const particle = document.createElement('span');
                particle.className = `particle-${i + 1}`;
                particle.textContent = emojis[Math.floor(Math.random() * emojis.length)];
                particle.style.position = 'absolute';
                particle.style.pointerEvents = 'none';
                particle.style.zIndex = '0';
                cardElement.appendChild(particle);
            }
        }
    },
    
    // ========================================
    // 🔊 SOUND EFFECTS (opcional)
    // ========================================
    sound: {
        enabled: false,
        
        play: function(type) {
            if (!this.enabled) return;
            
            // Usar Web Audio API para sons sutis
            const audioCtx = new (window.AudioContext || window.webkitAudioContext)();
            const oscillator = audioCtx.createOscillator();
            const gainNode = audioCtx.createGain();
            
            oscillator.connect(gainNode);
            gainNode.connect(audioCtx.destination);
            
            gainNode.gain.setValueAtTime(0.1, audioCtx.currentTime);
            
            switch(type) {
                case 'success':
                    oscillator.frequency.setValueAtTime(523.25, audioCtx.currentTime); // C5
                    oscillator.frequency.exponentialRampToValueAtTime(783.99, audioCtx.currentTime + 0.1); // G5
                    break;
                case 'error':
                    oscillator.frequency.setValueAtTime(200, audioCtx.currentTime);
                    break;
                case 'click':
                    oscillator.frequency.setValueAtTime(440, audioCtx.currentTime); // A4
                    break;
            }
            
            oscillator.start();
            oscillator.stop(audioCtx.currentTime + 0.1);
            
            gainNode.gain.exponentialRampToValueAtTime(0.01, audioCtx.currentTime + 0.1);
        },
        
        toggle: function() {
            this.enabled = !this.enabled;
            return this.enabled;
        }
    },
    
    // ========================================
    // 📳 HAPTIC FEEDBACK (mobile)
    // ========================================
    haptic: {
        vibrate: function(pattern = [50]) {
            if ('vibrate' in navigator) {
                navigator.vibrate(pattern);
            }
        },
        
        success: function() {
            this.vibrate([50, 50, 100]);
        },
        
        error: function() {
            this.vibrate([100, 50, 100, 50, 100]);
        },
        
        click: function() {
            this.vibrate([10]);
        }
    },
    
    // ========================================
    // 🎭 TOAST NOTIFICATIONS
    // ========================================
    toast: {
        show: function(message, type = 'success', duration = 3000) {
            const toast = document.createElement('div');
            toast.className = `toast-notification toast-${type} slide-in-down`;
            
            const icons = {
                success: '✅',
                error: '❌',
                info: 'ℹ️',
                warning: '⚠️'
            };
            
            toast.innerHTML = `
                <span class="toast-icon">${icons[type] || icons.info}</span>
                <span class="toast-message">${message}</span>
            `;
            
            toast.style.cssText = `
                position: fixed;
                top: 20px;
                right: 20px;
                background: linear-gradient(135deg, rgba(255, 255, 255, 0.95) 0%, rgba(232, 245, 233, 0.9) 100%);
                padding: 1rem 1.5rem;
                border-radius: 20px;
                box-shadow: 0 10px 30px rgba(0, 0, 0, 0.2);
                z-index: 10000;
                display: flex;
                align-items: center;
                gap: 0.75rem;
                min-width: 250px;
                backdrop-filter: blur(10px);
                border: 2px solid #A5D6A7;
            `;
            
            if (type === 'error') {
                toast.style.borderColor = '#EF9A9A';
            }
            
            document.body.appendChild(toast);
            
            setTimeout(() => {
                toast.style.animation = 'slideOutRight 0.4s ease-in forwards';
                setTimeout(() => toast.remove(), 400);
            }, duration);
        }
    },
    
    // ========================================
    // 🎯 FORM VALIDATION WITH EFFECTS
    // ========================================
    validation: {
        animateSuccess: function(input) {
            input.classList.add('is-valid');
            input.classList.remove('is-invalid');
            
            // Confetti mini no input
            const rect = input.getBoundingClientRect();
            const miniConfetti = document.createElement('span');
            miniConfetti.textContent = '✨';
            miniConfetti.style.cssText = `
                position: fixed;
                left: ${rect.right - 30}px;
                top: ${rect.top + rect.height / 2}px;
                font-size: 1.5rem;
                pointer-events: none;
                z-index: 1000;
                animation: confetti-pop 1s ease-out forwards;
            `;
            document.body.appendChild(miniConfetti);
            setTimeout(() => miniConfetti.remove(), 1000);
            
            // Som e vibração
            ErvilhinhaEpic.sound.play('success');
            ErvilhinhaEpic.haptic.click();
        },
        
        animateError: function(input) {
            input.classList.add('is-invalid');
            input.classList.remove('is-valid');
            
            // Shake animation
            input.style.animation = 'shake-intense 0.5s';
            setTimeout(() => {
                input.style.animation = '';
            }, 500);
            
            // Som e vibração
            ErvilhinhaEpic.sound.play('error');
            ErvilhinhaEpic.haptic.error();
        }
    },
    
    // ========================================
    // 🎬 SKELETON LOADER
    // ========================================
    skeleton: {
        show: function(container) {
            container.innerHTML = `
                <div class="skeleton skeleton-text" style="width: 70%;"></div>
                <div class="skeleton skeleton-text" style="width: 90%;"></div>
                <div class="skeleton skeleton-input"></div>
                <div class="skeleton skeleton-input"></div>
                <div class="skeleton skeleton-button"></div>
            `;
        },
        
        hide: function(container, content) {
            container.innerHTML = content;
        }
    },
    
    // ========================================
    // 🎯 AUTO-INIT
    // ========================================
    init: function(options = {}) {
        console.log('🎆 Ervilhinha Epic Effects - Initializing...');
        
        // Configurações padrão
        const config = {
            cursorTrail: options.cursorTrail !== false,
            sound: options.sound === true, // Desligado por padrão
            particles: options.particles !== false,
            haptic: options.haptic !== false,
            ...options
        };
        
        // Cursor trail (apenas desktop)
        if (config.cursorTrail && window.innerWidth > 768) {
            this.cursorTrail.enable();
        }
        
        // Adiciona partículas a cards modernos
        if (config.particles) {
            document.querySelectorAll('.form-card-modern').forEach(card => {
                this.particles.add(card);
            });
        }
        
        // Event listeners para botões modernos
        document.querySelectorAll('.btn-modern-primary, .btn-modern-outline').forEach(btn => {
            btn.addEventListener('click', () => {
                this.haptic.click();
                this.sound.play('click');
            });
        });
        
        // HTMX Integration
        if (typeof htmx !== 'undefined') {
            // Skeleton loader durante requests
            document.body.addEventListener('htmx:beforeRequest', (e) => {
                const indicator = e.detail.target.querySelector('.htmx-indicator');
                if (indicator) {
                    indicator.style.display = 'inline-block';
                }
            });
            
            // Success toast após submit bem-sucedido
            document.body.addEventListener('htmx:afterRequest', (e) => {
                if (e.detail.successful) {
                    this.toast.show('✅ Guardado com sucesso!', 'success');
                    this.confetti.launch(2000);
                    this.haptic.success();
                }
            });
        }
        
        // Form validation enhancement
        document.querySelectorAll('form').forEach(form => {
            form.addEventListener('submit', (e) => {
                const inputs = form.querySelectorAll('input[required], select[required], textarea[required]');
                let hasErrors = false;
                
                inputs.forEach(input => {
                    if (!input.value || input.value.trim() === '') {
                        this.validation.animateError(input);
                        hasErrors = true;
                    } else {
                        this.validation.animateSuccess(input);
                    }
                });
                
                if (hasErrors) {
                    e.preventDefault();
                    this.toast.show('⚠️ Preenche todos os campos obrigatórios', 'warning');
                }
            });
        });
        
        console.log('✨ Ervilhinha Epic Effects - Ready!');
    }
};

// ========================================
// 🚀 AUTO-INITIALIZE
// ========================================
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', () => {
        ErvilhinhaEpic.init({
            cursorTrail: true,
            sound: false, // Desligado por padrão - pode ativar com ErvilhinhaEpic.sound.toggle()
            particles: true,
            haptic: true
        });
    });
} else {
    ErvilhinhaEpic.init();
}

// Exponha globalmente
window.ErvilhinhaEpic = ErvilhinhaEpic;

// ========================================
// 📱 MOBILE DETECTION
// ========================================
function isMobile() {
    return /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent);
}

// ========================================
// 🎨 CSS INJECTION FOR ANIMATIONS
// ========================================
const style = document.createElement('style');
style.textContent = `
    @keyframes confetti-pop {
        0% {
            opacity: 1;
            transform: translateY(0) scale(1);
        }
        100% {
            opacity: 0;
            transform: translateY(-50px) scale(2);
        }
    }
    
    @keyframes slideOutRight {
        from {
            transform: translateX(0);
            opacity: 1;
        }
        to {
            transform: translateX(400px);
            opacity: 0;
        }
    }
    
    .toast-notification {
        font-family: 'Segoe UI', sans-serif;
        font-weight: 600;
        color: #5D4037;
    }
    
    .toast-icon {
        font-size: 1.5rem;
    }
    
    .htmx-indicator {
        display: none;
        margin-left: 0.5rem;
    }
    
    .htmx-indicator::after {
        content: '⏳';
        animation: spin 1s linear infinite;
    }
`;
document.head.appendChild(style);

console.log('🎆 Ervilhinha Epic - Loaded!');
