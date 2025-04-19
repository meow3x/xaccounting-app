import axios from "axios";

export default function createBasicClient(endpoint) {
  return {
    /**
     * Create a "getAll" client with pagination, filtering, & sorting support.
     * @param options
     * @returns {Promise<any>}
     */
    getAll: async (options = {}) => {
      return await defaultGetAll(endpoint, options);
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

export async function defaultGetAll(endpoint, options = {}) {
  const filters  = options.filters || []
  const pagination = options.pagination
  const searchParams = new URLSearchParams();

  if (filters.length > 0) {
    const searchTerms = filters.filter(e => isNonEmpty(e.value)).map(toQuerySyntax)
    if (searchTerms.length > 0) {
      searchParams.set('q', searchTerms.join(' AND '))
    }
  }

  if (pagination != null) {
    searchParams.set('page_id', pagination.page)
    searchParams.set('page_size', pagination.pageSize)
  }

  const result = await axios.get(endpoint + (searchParams.size > 0 ? `?${searchParams}` : ''))
  return {
    _meta: {
      total: parseInt(result.headers['x-pagination-total'])
    },
    records: result.data
  };
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