import { Api } from '@/api/Api';
import useAppStore from '@/stores/app';

export default function initializeStore() {
  const appStore = useAppStore();

  new Api()
    .appInfoList()
    .then((response) => appStore.setApplicationInfo(response.data))
    .catch((response) => appStore.setApiFailureMessages(response));
}
