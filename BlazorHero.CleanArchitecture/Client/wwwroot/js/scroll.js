window.ScrollToBottom = (elementName) => {
    scrollingElement = document.getElementById(elementName);
    scrollingElement.scrollTop = scrollingElement.scrollHeight;
}