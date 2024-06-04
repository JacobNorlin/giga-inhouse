import { useAuthContext } from "@giga-inhouse/components/auth-wrapper/use-auth-context";

function App() {
  const { user } = useAuthContext();

  return <div>{user.name}</div>;
}

export default App;
