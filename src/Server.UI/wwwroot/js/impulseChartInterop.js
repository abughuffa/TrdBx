// impulseChartInterop.js

let dotNetRef = null;
let resizeObserver = null;

/**
 * Initializes the impulse chart with .NET interop
 * @param {object} dotNetReference - The .NET object reference for callbacks
 */
function initializeImpulseChart(dotNetReference) {
    dotNetRef = dotNetReference;
    
    // Initialize interactions
    initializeImpulseInteraction();
    resizeImpulseChart();
    
    // Set up resize observer for responsive behavior
    setupResizeObserver();
    
    console.log('Impulse Chart initialized with .NET interop');
}

/**
 * Sets up a resize observer to handle container resizing
 */
function setupResizeObserver() {
    const container = document.querySelector('.impulse-container');
    if (!container) return;

    if (resizeObserver) {
        resizeObserver.disconnect();
    }

    resizeObserver = new ResizeObserver(() => {
        resizeImpulseChart();
        if (dotNetRef) {
            dotNetRef.invokeMethodAsync('OnResize');
        }
    });

    resizeObserver.observe(container);
}

/**
 * Scrolls the impulse chart container to a specific position
 * @param {number} position - The horizontal scroll position in pixels
 */
function scrollImpulseChartToPosition(position) {
    const container = document.querySelector('.impulse-container');
    if (!container) {
        console.warn('Impulse container not found');
        return;
    }

    const svg = container.querySelector('.impulse-svg');
    if (!svg) {
        console.warn('SVG element not found');
        return;
    }

    const maxScroll = Math.max(0, svg.scrollWidth - container.clientWidth);
    const clampedPosition = Math.max(0, Math.min(maxScroll, position));
    
    container.scrollTo({
        left: clampedPosition,
        behavior: 'smooth'
    });

    console.log(`Scrolled to position: ${clampedPosition}px (max: ${maxScroll}px)`);
}

/**
 * Gets the bounding rectangle of the impulse container
 * @returns {DOMRect|null} The bounding rectangle or null if not found
 */
function getImpulseContainerRect() {
    const container = document.querySelector('.impulse-container');
    if (!container) {
        console.warn('Impulse container not found');
        return null;
    }
    return container.getBoundingClientRect();
}

/**
 * Centers the impulse at a specific index in the viewport
 * @param {number} index - The index of the impulse to center
 * @param {number} itemSpacing - The spacing between items in pixels
 * @param {number} zoomLevel - The current zoom level
 */
function centerImpulseAtIndex(index, itemSpacing, zoomLevel) {
    const container = document.querySelector('.impulse-container');
    if (!container) {
        console.warn('Impulse container not found');
        return;
    }

    const impulseX = 80 + (index * itemSpacing);
    const centerOffset = container.clientWidth / 2;
    const scrollPosition = Math.max(0, impulseX - centerOffset);

    container.scrollTo({
        left: scrollPosition,
        behavior: 'smooth'
    });

    console.log(`Centered impulse ${index} at position: ${scrollPosition}px`);
}

/**
 * Checks if a specific impulse is visible in the viewport
 * @param {number} index - The index of the impulse
 * @param {number} itemSpacing - The spacing between items in pixels
 * @returns {boolean} True if the impulse is visible
 */
function isImpulseVisible(index, itemSpacing) {
    const container = document.querySelector('.impulse-container');
    if (!container) return false;

    const impulseX = 80 + (index * itemSpacing);
    const containerLeft = container.scrollLeft;
    const containerRight = containerLeft + container.clientWidth;

    return impulseX >= containerLeft - 20 && impulseX <= containerRight + 20;
}

/**
 * Calculates the optimal scroll position to show multiple items
 * @param {number} startIndex - The starting index
 * @param {number} endIndex - The ending index
 * @param {number} itemSpacing - The spacing between items in pixels
 * @param {number} padding - Additional padding in pixels
 * @returns {number} The optimal scroll position
 */
function calculateOptimalScrollPosition(startIndex, endIndex, itemSpacing, padding = 50) {
    const container = document.querySelector('.impulse-container');
    if (!container) return 0;

    const startX = 80 + (startIndex * itemSpacing);
    const endX = 80 + (endIndex * itemSpacing);
    const containerWidth = container.clientWidth;

    const rangeCenter = (startX + endX) / 2;
    let scrollPosition = rangeCenter - (containerWidth / 2);

    if (endX - startX < containerWidth - (padding * 2)) {
        scrollPosition = startX - padding;
    }

    const svg = container.querySelector('.impulse-svg');
    if (svg) {
        const maxScroll = Math.max(0, svg.scrollWidth - containerWidth);
        scrollPosition = Math.max(0, Math.min(maxScroll, scrollPosition));
    }

    return scrollPosition;
}

/**
 * Toggles the visibility of the tooltip for a specific impulse
 * @param {number} index - The index of the impulse
 * @param {boolean} show - True to show the tooltip, false to hide
 */
function toggleImpulseTooltip(index, show) {
    const impulseGroup = document.querySelector(`.impulse-group[data-index="${index}"]`);
    if (!impulseGroup) return;

    const tooltipElements = [
        '.tooltip-bg',
        '.tooltip-title',
        '.tooltip-value',
        '.tooltip-objects',
        '.tooltip-day'
    ];

    tooltipElements.forEach(selector => {
        const element = impulseGroup.querySelector(selector);
        if (element) {
            element.style.opacity = show ? '1' : '0';
        }
    });

    const valueLabel = impulseGroup.querySelector('.value-label');
    if (valueLabel) {
        valueLabel.style.opacity = show ? '1' : '0';
    }
}

/**
 * Gets the current visible range of impulses in the viewport
 * @param {number} itemSpacing - The spacing between items in pixels
 * @param {number} totalItems - Total number of items
 * @returns {Object} The visible range { startIndex, endIndex }
 */
function getVisibleImpulseRange(itemSpacing, totalItems) {
    const container = document.querySelector('.impulse-container');
    if (!container) return { startIndex: 0, endIndex: 0 };

    const containerLeft = container.scrollLeft;
    const containerRight = containerLeft + container.clientWidth;

    let startIndex = Math.max(0, Math.floor((containerLeft - 80) / itemSpacing));
    let endIndex = Math.min(totalItems - 1, Math.ceil((containerRight - 80) / itemSpacing));

    return { startIndex, endIndex };
}

/**
 * Snaps the scroll position to align impulses with the container
 * @param {number} itemSpacing - The spacing between items in pixels
 * @param {number} snapThreshold - The threshold for snapping in pixels
 */
function snapImpulseScroll(itemSpacing, snapThreshold = 20) {
    const container = document.querySelector('.impulse-container');
    if (!container) return;

    const currentScroll = container.scrollLeft;
    const offset = 80;

    const nearestIndex = Math.round((currentScroll - offset) / itemSpacing);
    const targetScroll = offset + (nearestIndex * itemSpacing);

    if (Math.abs(currentScroll - targetScroll) < snapThreshold) {
        container.scrollTo({
            left: targetScroll,
            behavior: 'smooth'
        });
    }
}

/**
 * Adds click handlers to impulse groups for better interaction
 * This should be called after the chart is rendered
 */
function initializeImpulseInteraction() {
    const impulseGroups = document.querySelectorAll('.impulse-group');
    
    impulseGroups.forEach(group => {
        group.removeEventListener('mouseenter', handleImpulseMouseEnter);
        group.removeEventListener('mouseleave', handleImpulseMouseLeave);
        group.removeEventListener('click', handleImpulseClick);
        
        group.addEventListener('mouseenter', handleImpulseMouseEnter);
        group.addEventListener('mouseleave', handleImpulseMouseLeave);
        group.addEventListener('click', handleImpulseClick);
    });
}

/**
 * Handle impulse mouse enter event
 * @param {MouseEvent} event - The mouse event
 */
function handleImpulseMouseEnter(event) {
    const group = event.currentTarget;
    const index = parseInt(group.dataset.index);
    
    showImpulseTooltip(group, true);
    
    if (dotNetRef) {
        dotNetRef.invokeMethodAsync('OnImpulseHover', index, true);
    }
}

/**
 * Handle impulse mouse leave event
 * @param {MouseEvent} event - The mouse event
 */
function handleImpulseMouseLeave(event) {
    const group = event.currentTarget;
    const index = parseInt(group.dataset.index);
    
    showImpulseTooltip(group, false);
    
    if (dotNetRef) {
        dotNetRef.invokeMethodAsync('OnImpulseHover', index, false);
    }
}

/**
 * Handle impulse click event
 * @param {MouseEvent} event - The mouse event
 */
function handleImpulseClick(event) {
    const group = event.currentTarget;
    const index = parseInt(group.dataset.index);
    
    if (dotNetRef) {
        dotNetRef.invokeMethodAsync('OnImpulseClicked', index);
    }
}

/**
 * Shows or hides the tooltip for an impulse group
 * @param {Element} group - The impulse group element
 * @param {boolean} show - True to show, false to hide
 */
function showImpulseTooltip(group, show) {
    const tooltipElements = [
        '.tooltip-bg',
        '.tooltip-title',
        '.tooltip-value',
        '.tooltip-objects',
        '.tooltip-day'
    ];

    tooltipElements.forEach(selector => {
        const element = group.querySelector(selector);
        if (element) {
            element.style.opacity = show ? '1' : '0';
        }
    });

    const valueLabel = group.querySelector('.value-label');
    if (valueLabel) {
        valueLabel.style.opacity = show ? '1' : '0';
    }
}

/**
 * Resizes the SVG to match the container width
 * This is useful when the window is resized
 */
function resizeImpulseChart() {
    const container = document.querySelector('.impulse-container');
    const svg = container?.querySelector('.impulse-svg');
    if (!container || !svg) return;

    const minWidth = Math.max(container.clientWidth, parseFloat(svg.style.minWidth) || 800);
    svg.style.width = Math.max(minWidth, svg.scrollWidth) + 'px';
    
    // Update the container height if needed
    const totalHeight = 300; // SVG height
    container.style.height = totalHeight + 'px';
}

/**
 * Gets the current zoom level from the SVG
 * @returns {number} The current zoom level
 */
function getCurrentZoomLevel() {
    const container = document.querySelector('.impulse-container');
    if (!container) return 1.0;

    const svg = container.querySelector('.impulse-svg');
    if (!svg) return 1.0;

    // Extract zoom from width percentage
    const widthStyle = svg.style.width || '';
    const match = widthStyle.match(/([\d.]+)%/);
    if (match) {
        return parseFloat(match[1]) / 100;
    }

    return 1.0;
}

// Export functions for global use
window.initializeImpulseChart = initializeImpulseChart;
window.scrollImpulseChartToPosition = scrollImpulseChartToPosition;
window.getImpulseContainerRect = getImpulseContainerRect;
window.centerImpulseAtIndex = centerImpulseAtIndex;
window.isImpulseVisible = isImpulseVisible;
window.calculateOptimalScrollPosition = calculateOptimalScrollPosition;
window.toggleImpulseTooltip = toggleImpulseTooltip;
window.getVisibleImpulseRange = getVisibleImpulseRange;
window.snapImpulseScroll = snapImpulseScroll;
window.initializeImpulseInteraction = initializeImpulseInteraction;
window.resizeImpulseChart = resizeImpulseChart;
window.getCurrentZoomLevel = getCurrentZoomLevel;

console.log('Impulse Chart JavaScript functions initialized successfully.');