import { Api } from '@/api/Api';
import useAppStore from '@/stores/app';

const appStore = useAppStore();

export default function initializeStore() {
  new Api()
    .appInfoList()
    .then((response) => appStore.setApplicationInfo(response.data))
    .catch((response) => appStore.setApiFailureMessages(response));
}
