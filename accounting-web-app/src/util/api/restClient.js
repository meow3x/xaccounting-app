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

export function toQuerySyntax(e) {
  const qValue = Array.isArray(e.value) ? '[' + e.value.join(',') + ']' : e.value
  return `${e.field}:${qValue}`
}

export function isNonEmpty(e) {
  if (typeof e == "string" && e.trim() === '') {
    return false
  } else if (Array.isArray(e) && e.length === 0) {
    return false
  }
  return true
}