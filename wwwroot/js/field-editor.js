let sortable = null;

export function init(containerId, dotNetRef) {
    const el = document.getElementById(containerId);
    if (!el) return;
    if (sortable) sortable.destroy();
    sortable = new Sortable(el, {
        handle: '.drag-handle',
        animation: 150,
        onEnd: function (evt) {
            const items = el.querySelectorAll('.field-block');
            const order = Array.from(items).map((item, index) => ({
                id: parseInt(item.dataset.fieldId),
                order: index
            }));
            dotNetRef.invokeMethodAsync('ReorderFields', order);
        }
    });
}

export function destroy() {
    if (sortable) {
        sortable.destroy();
        sortable = null;
    }
}
