import {notifications} from "@mantine/notifications";

export function showSuccessNotification(message) {
  notifications.show({
    title: 'Success',
    message,
    position: 'bottom-left',
    color: 'teal',
    withBorder: true
  })
}