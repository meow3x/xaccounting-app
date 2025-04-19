import {notifications} from "@mantine/notifications";

export function showSuccessNotification(message) {
  notifications.show({
    title: 'Success',
    message,
    position: 'bottom-right',
    color: 'teal',
    withBorder: true
  })
}