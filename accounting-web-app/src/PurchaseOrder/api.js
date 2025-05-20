import {useMutation, useQuery} from "@tanstack/react-query";
import axios from "axios";

export function useGetProjects() {
  return useQuery({
    queryKey: ['projects'],
    queryFn: async () => {
      const result = await axios.get('/api/PurchaseOrder/Projects')
      return result.data
    },
    staleTime: 1000 * 60 * 5
  })
}

export function useGetPoBalance() {
  return useQuery({
    queryKey: ['po-balance'],
    queryFn: async () => {
      const result = await axios.get('/api/PurchaseOrder/Balance')
      return result.data
    },
    staleTime: 1000 * 60 * 5

  })
}
export function useCreatePurchaseOrder() {
  return useMutation({
    mutationFn: async (e) => {
      const result = await axios.post('/api/PurchaseOrder', e)
      return result.data
    }
  })
}

export function useGetPurchaserOrder(orderNumber) {
  return useQuery({
    queryKey: ['purchaseOrder', orderNumber ],
    queryFn: async () => {
      const result = await axios.get(`/api/PurchaseOrder/${orderNumber}`)
      return result.data
    }
  })
}

export async function getPurchaseOrder(orderNumber) {
  const result = await axios.get(`/api/PurchaseOrder/${orderNumber}`)
  return result.data
}

export async function receivePurchaseOrder(id, data) {
  const result = await axios.post(`/api/PurchaseOrder/Receive/${id}`, data)
  return result.data
}