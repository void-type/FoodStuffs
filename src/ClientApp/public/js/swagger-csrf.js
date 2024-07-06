async function fetchAppInfo() {
  const response = await fetch('/api/app/info');
  const data = await response.json();
  window.vt_api_csrf_token = data.antiforgeryToken;
}

fetchAppInfo();
