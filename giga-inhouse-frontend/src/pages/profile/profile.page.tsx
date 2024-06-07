import { useAuthContext } from "@giga-inhouse/components/auth-wrapper/use-auth-context";
import { useMutateGigaInhouseUser } from "@giga-inhouse/hooks/use-mutate-giga-inhouse-user";
import {
  ActionIcon,
  Button,
  Flex,
  Loader,
  Mark,
  Popover,
  Stack,
  Text,
  TextInput,
} from "@mantine/core";
import { useForm } from "@mantine/form";
import { Info } from "@phosphor-icons/react";

export function ProfilePage() {
  const { user } = useAuthContext();
  const form = useForm({
    mode: "uncontrolled",
    initialValues: {
      userName: user.name,
      steamId: user.steamId,
    },
  });

  const mutateUser = useMutateGigaInhouseUser();

  return (
    <Stack gap="sm">
      <TextInput
        label="Username"
        placeholder="Enter username here"
        key={form.key("userName")}
        {...form.getInputProps("userName")}
      />
      <TextInput
        label={
          <Flex gap="sm" align="center" flex="1">
            <Text size="sm">Steam ID</Text>
            <Popover position="right" withArrow shadow="md">
              <Popover.Target>
                <ActionIcon>
                  <Info />
                </ActionIcon>
              </Popover.Target>
              <Popover.Dropdown>
                <Text size="xs">
                  https://steamcommunity.com/profiles/
                  <Mark>76561197996659158</Mark>/
                </Text>
              </Popover.Dropdown>
            </Popover>
          </Flex>
        }
        placeholder="Enter steam id here"
        key={form.key("steamId")}
        {...form.getInputProps("steamId")}
      />
      <Button
        onClick={() => {
          const values = form.getValues();
          mutateUser.mutate({
            userName: values.userName,
            steamId: values.steamId,
          });
        }}
      >
        Save {mutateUser.isPending ? <Loader /> : null}
      </Button>
    </Stack>
  );
}
