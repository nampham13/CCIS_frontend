(() => {
  const clock = document.querySelector('[data-clock]');
  const updateClock = () => {
    if (!clock) {
      return;
    }

    clock.textContent = new Date().toLocaleTimeString([], {
      hour: '2-digit',
      minute: '2-digit',
      second: '2-digit'
    });
  };

  updateClock();
  window.setInterval(updateClock, 1000);
})();