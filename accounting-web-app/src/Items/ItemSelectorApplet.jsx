import { Group, Stack, Select, NumberInput, TextInput, Button } from "@mantine/core";
import { useMemo, useState } from "react";
import { LineItemsList } from "../PurchaseOrder/PurchaseOrderForm";
import ItemBasicDetails from "./ItemBasicDetails";
import { IconPlus } from "@tabler/icons-react";
import { useGetItemInventoryQty } from "src/Inventory/api";


export function ItemSelectorApplet({ itemList, onItemAdded, isSelling }) {
  const [itemId, setItemId] = useState('');
  const [quantity, setQuantity] = useState(null);
  const [items, setItems] = useState([]);

  const selectedItem = useMemo(() => itemList?.find(e => e.id === parseInt(itemId)), [itemList, itemId])
  const { data: stockInfo, isFetching } = useGetItemInventoryQty(itemId)

  function handleAdd() {
    if (!(itemId && quantity)) {
      return;
    }

    const price = isSelling ? selectedItem.unitPrice : selectedItem.unitCost

    const newItem = {
      id: itemId,
      code: selectedItem.code,
      quantity: quantity,
      uom: selectedItem.uom.name,
      description: selectedItem.name,
      price: price,
      discount: 0,
      lineTotal: price * quantity,
    };

    onItemAdded?.({
      id: itemId,
      quantity: quantity,
      lineTotal: price * quantity
    });

    setItems(prev => [
      ...prev,
      newItem
    ]);
  }

  return (
    <>
      <Group align="flex-start" m="md">
        <Stack flex={1}>
          <Select
            required
            size="sm"
            label="Item"
            placeholder="Select item"
            searchable
            checkIconPosition="right"
            data={itemList?.map(e => ({
              value: e.id.toString(),
              label: `${e.code} - ${e.name}`
            }))}
            style={{ width: 400 }}
            value={itemId}
            onChange={setItemId} />

          {selectedItem
            ? <ItemBasicDetails item={selectedItem} quantityBalance={stockInfo?.quantity} isSelling={isSelling} />
            : <></>}

          <Group grow>
            <NumberInput
              required
              label="Quantity"
              value={quantity}
              onChange={setQuantity}
              min={1}
              max={stockInfo?.quantity ?? 0} />

            <NumberInput
              label="Discount" />
          </Group>

          <Button loading={isFetching} disabled={stockInfo?.quantity == 0} onClick={handleAdd} variant="outline" rightSection={<IconPlus size={16} />}>Add</Button>
        </Stack>

        <Stack flex={3}>
          <LineItemsList lineItems={items} isSelling={isSelling} />
        </Stack>
      </Group>
    </>
  );
}
