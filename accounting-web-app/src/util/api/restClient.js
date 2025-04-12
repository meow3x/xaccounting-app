import axios from "axios";

export default function createRestClient(endpoint) {
  return {
    getAll: async () => {
      const result = await axios.get(endpoint);
      return result.data;
    },
    getById: async (id) => {
      if (id == null) {
        throw new Error('Invalid id');
      }
      const result = await axios.get(`${endpoint}/${id}`);
      return result.data;
    },
    create: async (data) => {
      const result = await axios.post(endpoint, data);
      return result.data;
    },
    update: async (id, data)  => {
      const result = await axios.put(`${endpoint}/${id}`, data);
      return result.data
    },
    delete: async (id) => {}
  }
}