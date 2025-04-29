import { useAppDispatch, useAppSelector } from "../hooks/hooks";

function Navbar() {
  const { user } = useAppSelector((state) => state.account);
  const dispatch = useAppDispatch();

  return (
    <header
      className="bg-base-100 border-b border-base-300 fixed w-full top-0 z-40 
    backdrop-blur-lg"
    >
      <div className="container mx-auto px-4 h-16">
        <div className="flex items-center justify-between h-full">
          <div className="flex items-center gap-8">
            <a
              href="/"
              className="flex items-center gap-2.5 hover:opacity-80 transition-all"
            >
              <div className="size-9 rounded-lg bg-primary/10 flex items-center justify-center"></div>
              <h1 className="text-lg font-bold">MesajX</h1>
            </a>
          </div>

          {user && (
            <div className="flex items-center gap-2">
              <a href="/settings" className="btn btn-sm gap-2 transition-colors">
                <span className="hidden sm:inline">Settings</span>
              </a>

              <a href="/profile" className="btn btn-sm gap-2">
                <span className="hidden sm:inline">Profile</span>
              </a>

              <button
                className="btn btn-sm gap-2"
                onClick={() => {
                  // Logout işlemini buraya yazabilirsin
                  // dispatch(logout())
                }}
              >
                <span className="hidden sm:inline">Logout</span>
              </button>
            </div>
          )}
        </div>
      </div>
    </header>
  );
}

export default Navbar;
