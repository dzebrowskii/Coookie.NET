document.addEventListener('DOMContentLoaded', () => {
    animateLetters(document.querySelector('.coookie'), 0.1, 2.5);
    createGoldParticles(100);
});

function animateLetters(element, delay, duration) {
    let content = element.textContent;
    element.textContent = '';
    let letters = content.split('');

    letters.forEach((letter, index) => {
        let letterElement = document.createElement('span');
        letterElement.textContent = letter;
        letterElement.style.display = 'inline-block';
        letterElement.style.animation = `wave ${duration}s ease-in-out ${index * delay}s infinite`;
        element.appendChild(letterElement);
    });
}

function createGoldParticles(number) {
    for (let i = 0; i < number; i++) {
        let particle = document.createElement('div');
        particle.classList.add('gold-particle');
        particle.style.left = `${Math.random() * 100}vw`;
        particle.style.animationDuration = `${Math.random() * 3 + 5}s`; // od 5 do 8 sekund
        particle.style.animationDelay = `${Math.random() * 5}s`; // opóźnienie do 5 sekund
        document.body.appendChild(particle);
    }
}
