import axios from 'axios';

export default {
  url(id) {
    return `/api/images/${id}`;
  },
  upload(request, success, failure) {
    const formData = new FormData();
    Object.keys(request).forEach((key) => {
      formData.append(key, request[key]);
    });

    axios.post(
      '/api/images',
      formData,
      {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
      },
    )
      .then((response) => success(response.data))
      .catch((error) => failure(error.response));
  },
  delete(request, success, failure) {
    axios.delete('/api/images', { params: request })
      .then((response) => success(response.data))
      .catch((error) => failure(error.response));
  },
  pin(request, success, failure) {
    axios.post('/api/images/pin', request)
      .then((response) => success(response.data))
      .catch((error) => failure(error.response));
  },
};
