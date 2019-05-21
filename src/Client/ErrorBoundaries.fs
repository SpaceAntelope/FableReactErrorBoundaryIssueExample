
// based on https://github.com/fable-compiler/fable-react/blob/master/docs/react-error-boundaries.md
module ReactErrorBoundary 
    open Fable.React

    type [<AllowNullLiteral>] InfoComponentObject =
        abstract componentStack: string with get

    type ErrorBoundaryProps =
        { Inner : ReactElement
          ErrorComponent : ReactElement
          OnError : exn * InfoComponentObject -> unit }

    type ErrorBoundaryState =
        { HasErrors : bool }
    
    // See https://reactjs.org/docs/error-boundaries.html
    type ErrorBoundary(props) =
        inherit Component<ErrorBoundaryProps, ErrorBoundaryState>(props)
        do base.setInitState({ HasErrors = false })

        override x.componentDidCatch(error, info) =
            let info = info :?> InfoComponentObject
            x.props.OnError(error, info)
            x.setState(fun state props -> {state with HasErrors = true })

        override x.render() =
            if (x.state.HasErrors) then
                x.props.ErrorComponent
            else
                x.props.Inner

    let renderCatchSimple errorElement element =
        Browser.Dom.console.info("renderCatchSimple is called")
        ofType<ErrorBoundary,_,_> { Inner = element; ErrorComponent = errorElement; OnError = ignore } [ ]

    let renderCatchFn onError errorElement element =
        Browser.Dom.console.info("renderCatchFn is called")
        ofType<ErrorBoundary,_,_> { Inner = element; ErrorComponent = errorElement; OnError = onError } [ ]
