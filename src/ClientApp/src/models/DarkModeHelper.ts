const settingKeyEnableDarkMode = 'enableDarkMode';

export default class DarkModeHelper {
  static getInitialDarkModeSetting() {
    const appPreference = localStorage.getItem(settingKeyEnableDarkMode);

    if (appPreference) {
      return appPreference === 'true';
    }

    if (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches) {
      return true;
    }

    return false;
  }

  static setDarkMode(setting: boolean) {
    localStorage.setItem(settingKeyEnableDarkMode, setting.toString());

    const html = document.documentElement;

    if (setting) {
      html.setAttribute('data-bs-theme', 'dark');
      // For vue-color picker
      html.classList.add('dark');
    } else {
      html.removeAttribute('data-bs-theme');
      // For vue-color picker
      html.classList.remove('dark');
    }
  }
}
