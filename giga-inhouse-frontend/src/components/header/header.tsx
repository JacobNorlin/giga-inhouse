import { Box } from "@mantine/core";
import { useResizeObserver } from "@mantine/hooks";

export function Header() {
  const [resizeRef, rect] = useResizeObserver();

  return (
    <div style={{ width: "100%", height: "100%" }} ref={resizeRef}>
      <svg
        width={rect.width}
        height={rect.height}
        viewBox="0 0 100 100"
        preserveAspectRatio="xMaxYMax"
      >
        <rect x="0" y="0" width="100" height="100" fill="red" />
      </svg>
    </div>
  );
}
