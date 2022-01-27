let timers = document.getElementsByClassName('countdown-timer');

for (const t of timers) {
    let timerDiv = document.createElement('button');
    timerDiv.className = 'timer-element';
    let durationString = t.getAttribute('data-duration') || '00:05';
    timerDiv.innerHTML = durationString;
    let timer;
    timerDiv.addEventListener('click', (evt) => {
        if (!timer) {
            timer = setInterval(() => {
                let [min, sec] = timerDiv.innerHTML.split(':').map(d => +d);
                let oldTime = min * 60 + sec;
                if (oldTime > 0) {
                    let newTime = oldTime - 1;
                    let newMin = Math.floor(newTime / 60);
                    let newSec = Math.floor(newTime % 60);
                    newMin = newMin < 10 ? '0' + newMin : newMin;
                    newSec = newSec < 10 ? '0' + newSec : newSec;
                    timerDiv.innerText = `${newMin}:${newSec}`;
                } else {
                    timerDiv.style.opacity = '50%';
                    clearInterval(timer);
                }
            }, 1000);
        }
    });
    t.appendChild(timerDiv);
}