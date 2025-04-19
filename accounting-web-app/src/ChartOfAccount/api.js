import {useEffect, useState} from "react";
import axios from "axios";
import {isNonEmpty, toQuerySyntax} from "src/util/api/restClient.js";

export async function getAccountTypes() {
  const result = await axios.get('/api/ChartOfAccounts/AccountTypes');
  return result.data;
}

export async function createAccount(account) {
  const result = await axios.post(`/api/ChartOfAccounts`, account);
  return result.data;
}

export async function getAccounts(config = {}) {
  const filters  = config.filters || []
  const pagination = config.pagination
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

  const result = await axios.get('/api/ChartOfAccounts' + (searchParams.size > 0 ? `?${searchParams}` : ''))
  return {
    _meta: {
      total: parseInt(result.headers['x-pagination-total'])
    },
    data: result.data
  };
}

export async function getAccount(id) {
  if (id === undefined) {
    throw new Error("Invalid account id");
  }
  const result = await axios.get(`/api/ChartOfAccounts/${id}`);
  return result.data;
}

export async function updateAccount(id, account) {
  const result = await axios.put(`/api/ChartOfAccounts/${id}`, account);
  return result.data;
}

export function useAccountTypes() {
  const [accountTypes, setAccountTypes] = useState([])
  useEffect(() => {
    getAccountTypes(accountTypes)
      .then(setAccountTypes)
  }, [])
  return accountTypes
}

export function useAccount(id, handlerFn) {
  const [account, setAccount] = useState(null);
  useEffect(() => {
    if (id != null) {
      getAccount(id)
        .then(e => {
          setAccount(e);
          handlerFn?.(e);
        })
    }
  }, [id]);
  return account;
}